﻿using BBBCSIO;
using System;
using System.Collections.Generic;

namespace Scarlet.IO.BeagleBone
{
    public static class SPIBBB
    {
        public static SPIBusBBB SPIBus0 { get; private set; }
        public static SPIBusBBB SPIBus1 { get; private set; }

        static internal void Initialize(bool Enable0, bool Enable1)
        {
            if (Enable0) { SPIBus0 = new SPIBusBBB(0); }
            if (Enable1) { SPIBus1 = new SPIBusBBB(1); }
        }
    }

    public class SPIBusBBB : ISPIBus
    {

        private ScarletSPIPortFS Port;
        //private Dictionary<BBBPin, SPISlaveDeviceHandle> Devices;

        internal SPIBusBBB(byte ID)
        {
            switch (ID)
            {
                case 0: this.Port = new ScarletSPIPortFS(SPIPortEnum.SPIPORT_0); break;
                case 1: this.Port = new ScarletSPIPortFS(SPIPortEnum.SPIPORT_1); break;
                default: throw new ArgumentOutOfRangeException("Only SPI ports 0 and 1 are supported.");
            }
        }

        public void Initialize()
        {
            //this.Devices = new Dictionary<BBBPin, SPISlaveDeviceHandle>();
            //this.Devices.Add(BBBPin.NONE, this.Port.EnableSPISlaveDevice(SPISlaveDeviceEnum.SPI_SLAVEDEVICE_CS1));
            this.Port.SetMode(SPIModeEnum.SPI_MODE_0);
            this.Port.SetDefaultSpeedInHz(500000); // TODO: Determine appropriate speed.
        }

        /*private SPISlaveDeviceHandle GetOrCreateHandle(BBBPin CSPin)
        {
            if (!this.Devices.ContainsKey(CSPin) || this.Devices[CSPin] == null) { this.Devices.Add(CSPin, this.Port.EnableSPIGPIOSlaveDevice(Pin.PinToGPIO(CSPin))); }
            return this.Devices[CSPin];
        }*/

        public byte[] Write(IDigitalOut DeviceSelect, byte[] Data, int DataLength)
        {
            byte[] ReceivedData = new byte[DataLength];
            //this.Port.SPITransfer(GetOrCreateHandle(DeviceSelect), Data, ReceivedData, DataLength);
            this.Port.SPITransfer(DeviceSelect, Data, ReceivedData, DataLength);
            return ReceivedData;
        }

        public void Dispose()
        {
            this.Port.ClosePort();
            this.Port.Dispose();
        }
    }
}
