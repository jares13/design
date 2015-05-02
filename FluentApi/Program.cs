using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace FluentTask
{
	internal class Program
	{
		private static void Main()
		{
			var behaviour = new Behavior()
				.Say("Привет мир!")
				.UntilKeyPressed(b => b
					.Say("Ля-ля-ля!")
					.Say("Тру-лю-лю"))
				.Jump(JumpHeight.High)
				.UntilKeyPressed(b => b
					.Say("Aa-a-a-a-aaaaaa!!!")
					.Say("[набирает воздух в легкие]"))
				.Say("Ой!")
				.Delay(TimeSpan.FromSeconds(1))
				.Say("Кто здесь?!")
				.Delay(TimeSpan.FromMilliseconds(2000));

			behaviour.Execute();


		}
	}

    internal class Behavior
    {

        private List<IAction> actions = new List<IAction>();

        public Behavior Say(string text)
        {
            actions.Add(new ActionSay(text));
            return this;
        }

        public Behavior UntilKeyPressed(Func<Behavior, Behavior> func)
        {
            actions.Add(new ActionUntilKeyPressed(func));
            return this;
        }

        public Behavior Jump(JumpHeight high)
        {
            var text = high == JumpHeight.High ? "jump up" : "jump down";
            actions.Add(new ActionSay(text));
            return this;
        }

        public Behavior Delay(TimeSpan fromSeconds)
        {
            actions.Add(new ActionDelay(fromSeconds));
            return this;
        }

        public void Execute()
        {
            actions.ForEach(action => action.Execute());
        }
    }

    interface IAction
    {
        void Execute();
    }

    class ActionSay : IAction
    {

        private string text;

        public ActionSay(string text)
        {
            this.text = text;
        }

        public void Execute()
        {
            Console.WriteLine(text);
        }
    }

    class ActionDelay : IAction
    {
        private TimeSpan delay;

        public ActionDelay(TimeSpan delay)
        {
            this.delay = delay;
        }

        public void Execute()
        {
            System.Threading.Thread.Sleep(delay);
        }
    }

    class ActionUntilKeyPressed : IAction
    {
        private Func<Behavior, Behavior> func;
        public ActionUntilKeyPressed(Func<Behavior, Behavior> func)
        {
            this.func = func;
        }

        public void Execute()
        {
            while (!Console.KeyAvailable)
            {
                var delay = new TimeSpan(0, 0, 0, 1);
                System.Threading.Thread.Sleep(delay);
                var behaviour = new Behavior();
                behaviour = func(behaviour);
                behaviour.Execute();
            }
            Console.ReadKey();
        }
    }


}
