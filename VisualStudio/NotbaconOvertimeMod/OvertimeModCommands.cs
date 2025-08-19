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
    }
}
