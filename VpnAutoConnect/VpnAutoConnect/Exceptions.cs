using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VpnAutoConnect
{
    /// <summary>
    /// 自動ログイン動作を継続できない種類の例外
    /// </summary>
    public class CancelAutoPilotException: Exception
    {
    }

    /// <summary>
    /// 自動ログインを継続できる種類の例外
    /// </summary>
    public class AutoPilotContinueException : Exception
    {
    }
}
