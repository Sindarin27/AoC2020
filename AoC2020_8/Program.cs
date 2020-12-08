using System;
using System.Collections.Generic;

namespace AoC2020_8
{
    class Program
    {
        static void Main(string[] args)
        {
            string line = Console.ReadLine();
            List<Command> commands = new List<Command>(); // A list is more efficient if we need to add a lot of elements
            
            while (!string.IsNullOrEmpty(line))
            {
                string[] parts = line.Split(' ');
                commands.Add(ReadCommand(parts));
                
                line = Console.ReadLine();
            }
            
            Machine machine = new Machine(commands.ToArray());
            Console.WriteLine("Accumulator at " + machine.RunNoRepeat() + " after single loop");
            Console.WriteLine("Replace statement at ln " + machine.FindWrongInstruction() + 
                              " to terminate at " + machine.RunNoRepeat());
        }

        private static Command ReadCommand(IReadOnlyList<string> parts)
        {
            int argument = int.Parse(parts[1]);
            switch (parts[0])
            {
                case "acc":
                    return new CmdAccumulate(argument);
                case "jmp":
                    return new CmdJump(argument);
                default:
                    return new CmdNoOperation(argument);
            }
        }
    }

    class Machine
    {
        public int opCounter = 0;
        public int accumulator = 0;
        private readonly Command[] code;
        
        public Machine(Command[] code)
        {
            this.code = code;
        }

        public void Reset()
        {
            accumulator = 0;
            opCounter = 0;
            foreach (Command cmd in code)
            {
                cmd.executedBefore = false;
            }
        }

        public int RunNoRepeat()
        {
            while (opCounter >= 0 && opCounter < code.Length)
            {
                Command run = code[opCounter];
                // Console.WriteLine(run.ToString() + " ln " + opCounter);
                if (run.executedBefore) return accumulator;
                run.Execute(this);
            }

            // Console.WriteLine("Code finished running.");
            return accumulator;
        }

        public bool CheckRepeat()
        {
            while (opCounter >= 0 && opCounter < code.Length)
            {
                Command run = code[opCounter];
                // Console.WriteLine(run.ToString() + " ln " + opCounter);
                if (run.executedBefore) return true;
                run.Execute(this);
            }

            // Console.WriteLine("Code finished running.");
            return false;
        }

        public int FindWrongInstruction()
        {
            for (int i = 0; i < code.Length; i++)
            {
                if (code[i] is CmdAccumulate) continue;
                SwapCodeAt(i);
                if (!CheckRepeat()) return i;
                SwapCodeAt(i);
                Reset();
            }

            return -1;
        }

        // Swap the command at index
        // Replaces a jump with a no-op and a no-op with a jump
        public void SwapCodeAt(int index)
        {
            Command original = code[index];
            switch (original)
            {
                case CmdJump _:
                    code[index] = new CmdNoOperation(original.argument);
                    break;
                case CmdNoOperation _:
                    code[index] = new CmdJump(original.argument);
                    break;
            }
        }
    }

    #region commands

    abstract class Command
    {
        public readonly int argument;
        public bool executedBefore = false;

        protected Command(int argument)
        {
            this.argument = argument;
        }

        public virtual void Execute(Machine executor)
        {
            executor.opCounter++;
            executedBefore = true;
        }
    }

    class CmdAccumulate : Command
    {
        public CmdAccumulate(int argument) : base(argument)
        {
        }

        public override void Execute(Machine executor)
        {
            executor.accumulator += argument;
            base.Execute(executor);
        }
    }

    class CmdJump : Command
    {
        public CmdJump(int argument) : base(argument)
        {
        }

        public override void Execute(Machine executor)
        {
            executor.opCounter += argument;
            executedBefore = true;
        }
    }

    class CmdNoOperation : Command
    {
        public CmdNoOperation(int argument) : base(argument)
        {
        }
    }

    #endregion
}