using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

public class AIMBOT_HEAD
{
    private static string AimbotScan = "FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? 00 00 00 00 00 00 00 00 00 00 00 00 A5 43";
    private static string headoffset = "0xAA";
    private static string chestoffset = "0xA6";

    private static Dictionary<long, int> OrginalValues1 = new Dictionary<long, int>();
    private static Dictionary<long, int> OrginalValues2 = new Dictionary<long, int>();
    private static Dictionary<long, int> OrginalValues3 = new Dictionary<long, int>();
    private static Dictionary<long, int> OrginalValues4 = new Dictionary<long, int>();

    public static async Task Run(dynamic PLAYBOX, dynamic PID, bool isMuted = false)
    {
        OrginalValues1.Clear();
        OrginalValues2.Clear();
        OrginalValues3.Clear();
        OrginalValues4.Clear();

        PID.Text = "Applying...";
        Int64 readoffset = Convert.ToInt64(headoffset, 16);
        Int64 writeoffset = Convert.ToInt64(chestoffset, 16);

        try
        {
            Int32 proc = Process.GetProcessesByName("HD-Player")[0].Id;
            PLAYBOX.OpenProcess(proc);

            var result = await PLAYBOX.AoBScan2(AimbotScan, true, true);
            if (result.Count() != 0)
            {
                foreach (var CurrentAddress in result)
                {
                    Int64 addressToSave = CurrentAddress + writeoffset;
                    var currentBytes = PLAYBOX.readMemory(addressToSave.ToString("X"), sizeof(int));
                    int currentValue = BitConverter.ToInt32(currentBytes, 0);
                    OrginalValues1[addressToSave] = currentValue;

                    Int64 addressToSave9 = CurrentAddress + readoffset;
                    var currentBytes9 = PLAYBOX.readMemory(addressToSave9.ToString("X"), sizeof(int));
                    int currentValue9 = BitConverter.ToInt32(currentBytes9, 0);
                    OrginalValues2[addressToSave9] = currentValue9;

                    Int64 headbytes = CurrentAddress + readoffset;
                    Int64 chestbytes = CurrentAddress + writeoffset;

                    var bytes = PLAYBOX.readMemory(headbytes.ToString("X"), sizeof(int));
                    int Read = BitConverter.ToInt32(bytes, 0);

                    var bytes2 = PLAYBOX.readMemory(chestbytes.ToString("X"), sizeof(int));
                    int Read2 = BitConverter.ToInt32(bytes2, 0);

                    PLAYBOX.WriteMemory(chestbytes.ToString("X"), "int", Read.ToString());
                    PLAYBOX.WriteMemory(headbytes.ToString("X"), "int", Read2.ToString());

                    Int64 addressToSave1 = CurrentAddress + writeoffset;
                    var currentBytes1 = PLAYBOX.readMemory(addressToSave9.ToString("X"), sizeof(int));
                    int currentValue1 = BitConverter.ToInt32(currentBytes1, 0);
                    OrginalValues3[addressToSave1] = currentValue1;

                    Int64 addressToSave19 = CurrentAddress + readoffset;
                    var currentBytes19 = PLAYBOX.readMemory(addressToSave19.ToString("X"), sizeof(int));
                    int currentValue19 = BitConverter.ToInt32(currentBytes19, 0);
                    OrginalValues4[addressToSave19] = currentValue19;
                }

                PID.Text = "AIMBOT ACTIVATED";
                if (!isMuted) Console.Beep(1000, 400);
            }
            else
            {
                PID.Text = "AIMBOT FAILED";
                if (!isMuted) Console.Beep(2000, 400);
            }
        }
        catch (Exception ex)
        {
            PID.Text = "Patch Failed: " + ex.Message;
        }
    }
}
