using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

public class AWM_Patch
{
    public static async Task Run(dynamic mem, Label PID)
    {
        try
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Console.Beep(1000, 100); // শুরুতে beep
            PID.Text = "⏳ Searching for AWM value...";

            Int32 proc = Process.GetProcessesByName("HD-Player")[0].Id;
            mem.OpenProcess(proc);

            var result = await mem.AoBScan("0A D7 A3 3D 00 00 00 00 00 00 5C 43 00 00 90 42 00 00 B4 42 96 00 00 00 00 00 00 00 00 00 00 3F 00 00 80 3E 00 00 00 00 04 00 00 00 00 00 80 3F 00 00 20 41 00 00 34 42 01 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 0A D7 23 3F 9A 99 99 3F 00 00 80 3F 00 00 00 00 00 00 80 3F 00 00 80 3F 00 00 80 3F 00 00 00 00 00 00 00 00 00 00 00 3F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 00 00 80 3F 00 00 80 3F 00 00").ConfigureAwait(false);

            PID.Text = $"📦 Found: {result.Count()} Match(es)";

            if (result.Count() == 0)
            {
                MessageBox.Show("❌ AWM value not found. Try in lobby or check pattern.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (result.Count() > 2)
            {
                MessageBox.Show("⚠️ Too many results found. Not safe to patch.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (long address in result)
            {
                PID.Text = $"✏️ Patching at: 0x{address:X}";
                mem.WriteMemory(address.ToString("X"), "bytes", "0A D7 A3 3D 00 00 00 00 00 00 5C 43 00 00 90 42 00 00 B4 42 96 00 00 00 00 00 00 00 EC 51 B8 3D 8F C2 F5 3C 00 00 00 00 04 00 00 00 00 00 80 3F 00 00 20 41 00 00 34 42 01 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 0A D7 23 3F 9A 99 99 3F 00 00 80 3F 00 00 00 00 00 00 80 3F 00 00 80 3F 00 00 80 3F 00 00 00 00 00 00 00 00 00 00 00 3F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 00 00 80 3F 00 00 80 3F 00 00", string.Empty, null);
                Console.Beep(1200, 150); // patched এর পর beep
            }

            stopwatch.Stop();
            PID.Text = $"✅ AWM Fast Switch: ON (⏱ {stopwatch.Elapsed.TotalSeconds:F2}s)";
        }
        catch (Exception ex)
        {
            MessageBox.Show("❌ Error: " + ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
