using System;

namespace Procons.Durrah.Main
{
   public static class ExtentionMethods
    {
        public static void ReleaseObject(this object ob)
        {
            System.Runtime.InteropServices.Marshal.ReleaseComObject(ob);
            ob = null;
            
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
