using BlueApps.MaterialFlow.Common.Connection.Packets;
using BlueApps.MaterialFlow.Common.Connection.PackteHelper;
using BlueApps.MaterialFlow.Common.Values.Types;
using System.Text;
using BlueApps.MaterialFlow.Common.Utils;

namespace BlueApps.MaterialFlow.Common.Connection.PacketHelper;

public abstract class PLC_MessagePacketHelper : MessagePacketHelper
{
    public override string InTopic { get; set; }
    public override string OutTopic { get; set; }
    /// <summary>
    /// First area will be override by command. The second area will be override by random id.
    /// </summary>
    public List<string> Areas { get; protected set; }
    public IPacketSettings PacketSettings { get; private set; }


    private PLC_Command _command;
    private string _packetId;

    public PLC_Command Command
    {
        get => _command;

        set
        {
            _command = value;
            Areas[0] = _command.ToString();
        }
    }

    public string PacketId
    {
        get => _packetId;

        private set
        {
            _packetId = value;
            Areas[1] = _packetId;
        }
    }

    protected PLC_MessagePacketHelper(IPacketSettings settings) //Packetsettings in 152004 soll von IPackt.. erben...
    {
        PacketSettings = settings;
        Areas = new List<string>(settings.AreaLengths.Length);

        foreach (var s in settings.AreaLengths)
            Areas.Add("");

        CreatePacketId();
    }

    public void CreatePacketId() => PacketId = StringUtils.GetRandomString(PacketSettings?.AreaLengths[1] ?? 5);

    public void ClearAreas()
    {
        Areas?.Clear();

        foreach (var s in PacketSettings.AreaLengths)
            Areas.Add("");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="NullReferenceException"></exception>
    public override MessagePacket GetPacketData()
    {
        if (!SettingsIsValid())
        {
            throw new InvalidOperationException("A datastring cannot be created as long settings is not valid.");
        }

        var data = GetCommandLine();

        if (string.IsNullOrWhiteSpace(data))
        {
            //TODO: Log and ?
        }

        return new MessagePacket { Data = data, Topic = OutTopic };
    }

    private bool SettingsIsValid()
    {
        if (PacketSettings is null)
            return false;

        return PacketSettings.MaxStringLength > 0 && PacketSettings.AreaLengths != null && PacketSettings.AreaLengths.Length > 0
               && !char.IsWhiteSpace(PacketSettings.FillChar) && !char.IsWhiteSpace(PacketSettings.Delimeter);
    }

    private string GetCommandLine()
    {
        var data = new StringBuilder();

        if (Areas != null)
        {
            for (var i = 0; i < PacketSettings.MaxAreaLength; i++)
            {
                if (string.IsNullOrWhiteSpace(Areas[i]))
                {
                    data.Append(FillArea(PacketSettings.AreaLengths[i])).Append(PacketSettings.Delimeter);
                }
                else
                {
                    data.Append(FillArea(PacketSettings.AreaLengths[i], Areas[i])).Append(PacketSettings.Delimeter);
                }
            }

            data.Length--; //this will remove the last ';'

            return data.ToString();
        }

        return string.Empty;
    }

    private string FillArea(int areaLength, string value = "")
    {
        var sb = new StringBuilder(areaLength);

        if (string.IsNullOrEmpty(value))
        {
            for (var i = 0; i < areaLength; i++)
            {
                sb.Append(PacketSettings.FillChar);
            }
        }
        else
        {
            var difLength = areaLength - value.Length;

            sb.Append(value); //values on the left side

            for (var i = 0; i < difLength; i++)
            {
                sb.Append(PacketSettings.FillChar);
            }
        }

        return sb.ToString();
    }

    public override void SetPacketData(MessagePacket message)
    {
        if (string.IsNullOrWhiteSpace(message.Data))
            return; //TODO: Log.

        string[] dataAsArray = message.Data.Split(PacketSettings.Delimeter);

        try
        {
            ValidateData(dataAsArray);

            ConvertToAreas(dataAsArray);
        }
        catch (InvalidOperationException invEx)
        {
            //TODO: Log & other 
        }
        catch (Exception ex)
        {
            //TODO: Log...
        }
    }

    private void ValidateData(string[] dataAsArray)
    {
        if (dataAsArray.Length != PacketSettings.MaxAreaLength)
        {
            //TODO: Log...
        }
    }

    private void ConvertToAreas(string[] dataAsArray)
    {
        SetCommand(dataAsArray[0]);

        for (var i = 1; i < dataAsArray.Length; i++)
        {
            Areas[i] = dataAsArray[i].Trim(PacketSettings.FillChar);
        }
    }

    private void SetCommand(string firstAreaValue)
    {
        var cmdAsString = firstAreaValue.Trim(PacketSettings.FillChar);

        foreach (var cmd in (PLC_Command[])Enum.GetValues(typeof(PLC_Command)))
        {
            if (cmd.ToString().Equals(cmdAsString))
            {
                _command = cmd;
                Areas[0] = cmdAsString;
                return;
            }
        }

        throw new InvalidOperationException($"The string-command ({firstAreaValue}) is invalid.");
    }

    public override string ToString() => Areas is not null ? $"{string.Join(";", Areas)}" : "-";
}