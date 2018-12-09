using System;
using System.IO;
using GAssistant.Config;
using Newtonsoft.Json;

namespace GAssistant.Device
{
    public class DeviceRegister
    {
        private DeviceRegisterConf deviceRegisterConf;

        private DeviceModel deviceModel;

        private DeviceDesc device;

        private DeviceInterface deviceInterface;

        public DeviceRegister(DeviceRegisterConf deviceRegisterConf, string accessToken)
        {
            this.deviceRegisterConf = deviceRegisterConf;

            deviceInterface = new DeviceInterface(deviceRegisterConf.apiEndpoint, accessToken);
        }

        public void Register()
        {
            string projectId = deviceRegisterConf.projectId;

            deviceModel = RegisterModel(projectId);

            device = RegisterInstance(projectId, deviceModel.deviceModelId);
        }

        public DeviceModel GetDeviceModel()
        {
            return deviceModel;
        }

        public DeviceDesc GetDevice()
        {
            return device;
        }

        private T ReadFromFile<T>(string filePath, Type targetClass)
        {
            if (File.Exists(filePath)) {
                try
                {
                    using (StreamReader file = File.OpenText(filePath))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        return (T)serializer.Deserialize(file, typeof(T));
                    }
                }
                catch (IOException e)
                {
                    Logger.Get().Warning("Unable to read the content of the file " + e);
                }
            }

            return (T)(Object)null;
        }

        private DeviceModel RegisterModel(string projectId) {

            DeviceModel optionalDeviceModel = ReadFromFile<DeviceModel>(deviceRegisterConf.deviceModelFilePath, typeof(DeviceModel));
            if (optionalDeviceModel != null)
            {
                return optionalDeviceModel;
            }

            // If we can't get the device model from a file, continue with the webservice
            string modelId = projectId + Guid.NewGuid();

            DeviceModel.Manifest manifest = new DeviceModel.Manifest();
            manifest.manufacturer = "Tony";
            manifest.productName  = "Assistant SDK Demo";
            manifest.deviceDescription = "Assistant SDK Demo in CSharp";

            DeviceModel devModel = new DeviceModel();
            devModel.deviceModelId = modelId;
            devModel.projectId = projectId;
            devModel.name = string.Format("projects/{0}/deviceModels/{1}", projectId, modelId);
            devModel.deviceType = "action.devices.types.AUTO";
            devModel.manifest = manifest;

            try {
                Logger.Get().Debug("Creating device model");
                DeviceModel response = deviceInterface.RegisterModel(projectId, devModel);

                if (response != null) {
                    using (StreamWriter file = File.CreateText(deviceRegisterConf.deviceModelFilePath))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Formatting = Formatting.Indented;
                        serializer.Serialize(file, response);
                    }
                    return response;
                }
            } catch (IOException e) 
            {
                Logger.Get().Error("Error during RegisterModel : " + e);
            }

            return null;
        }


        private DeviceDesc RegisterInstance(string projectId, string modelId)
        {
            DeviceDesc optionalDevice = ReadFromFile<DeviceDesc>(deviceRegisterConf.deviceInstanceFilePath, typeof(DeviceDesc));
            if (optionalDevice != null)
            {
                return optionalDevice;
            }

            DeviceDesc dev = new DeviceDesc();
            dev.id = Guid.NewGuid().ToString();
            dev.modelId = modelId;

            // Here we use the Google Assistant Service
            dev.clientType = "SDK_SERVICE";

            try {
                DeviceDesc response = deviceInterface.RegisterDevice(projectId, dev);
                if (response != null)
                {
                    using (StreamWriter file = File.CreateText(deviceRegisterConf.deviceInstanceFilePath))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Formatting = Formatting.Indented;
                        serializer.Serialize(file, response);
                    }
                    return response;
                }
            } catch (IOException e) {
                Logger.Get().Error("Error during RegisterInstance : " + e);
            }

            return null;
        }
    }
}
