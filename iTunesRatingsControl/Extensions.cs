using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace iTunesRatingsControl {
    public static class Extensions {
        public static void InvokeIfRequired(this ISynchronizeInvoke obj, MethodInvoker action) {
            try {
                if (obj.InvokeRequired) {
                    var args = new object[0];
                    obj.Invoke(action, args);
                }
                else {
                    action();
                }
            }
            catch {
                // do nothing.
            }
        }
        public static void AsyncInvokeIfRequired(this ISynchronizeInvoke obj, MethodInvoker action) {
            try {
                if (obj.InvokeRequired) {
                    var args = new object[0];
                    obj.BeginInvoke(action, args);
                }
                else {
                    action();
                }
            }
            catch {
                // do nothing.
            }
        }
    }
}