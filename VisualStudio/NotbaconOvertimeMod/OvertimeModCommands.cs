using System;
using System.Collections.Generic;

namespace NotbaconOvertimeMod
{
    public class OvertimeModCommands : ConsoleCommandsBase
    {
        public override void SetList()
        {
            base.SetList();
            agentCommand.Add("markfordeath");
        }

        public override void AgentCommandOperation(int index, params string[] param)
        {
            switch (index)
            {
                case 0:
                    {
                        foreach (WorkerModel worker in GetListOfWorkers(param[0]))
                        {
                            worker.AddUnitBuf(new NotbaconPaleHorseMarkedBuf());
                        }
                    }
                    break;
            }
        }
    }
}
