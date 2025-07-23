using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

public class AWM_Patch
{
    public static async Task Run(dynamic mem, Label PID)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        Console.Beep(100, 200);
        PID.Text = "ᴀᴘᴘʟʏɪɴɢ AWM SWITCH";

        Int32 proc = Process.GetProcessesByName("HD-Player")[0].Id;
        mem.OpenProcess(proc);

        var result = await mem.AoBScan("0A D7 A3 3D 00 00 00 00 00 00 5C 43 00 00 90 42 00 00 B4 42 96 00 00 00 00 00 00 00 00 00 00 3F 00 00 80 3E 00 00 00 00 04 00 00 00 00 00 80 3F 00 00 20 41 00 00 34 42 01 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 0A D7 23 3F 9A 99 99 3F 00 00 80 3F 00 00 00 00 00 00 80 3F 00 00 80 3F 00 00 80 3F 00 00 00 00 00 00 00 00 00 00 00 3F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 00 00 80 3F 00 00 80 3F 00 00");

        if (result.Count() != 0 && result.Count() < 2)
        {
            foreach (long num in result)
            {
                mem.WriteMemory(num.ToString("X"), "bytes", "0A D7 A3 3D 00 00 00 00 00 00 5C 43 00 00 90 42 00 00 B4 42 96 00 00 00 00 00 00 00 EC 51 B8 3D 8F C2 F5 3C 00 00 00 00 04 00 00 00 00 00 80 3F 00 00 20 41 00 00 34 42 01 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 0A D7 23 3F 9A 99 99 3F 00 00 80 3F 00 00 00 00 00 00 80 3F 00 00 80 3F 00 00 80 3F 00 00 00 00 00 00 00 00 00 00 00 3F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 00 00 80 3F 00 00 80 3F 00 00", string.Empty, null);
            }
        }

        stopwatch.Stop();
        double elapsedSeconds = stopwatch.Elapsed.TotalSeconds;
        Console.Beep(200, 300);
        PID.Text = $"AWM Switch=ᴏɴ,ᴛꞮᴍᴇ: {elapsedSeconds:F2} Seconds";

        if (result.Count() > 2)
        {
            MessageBox.Show("ᴛʜꞮꜱ ᴄᴏᴅᴇ ɴᴏᴛ ꜱᴀꜰᴇ.", "ᴇƦƦᴏƦ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
