using System;
using System.IO;
using googleassistantcsharpdemo.config;
using Newtonsoft.Json;

namespace googleassistantcsharpdemo.device
{
    public class DeviceRegister
    {
        private DeviceRegisterConf deviceRegisterConf;

        private DeviceModel deviceModel;

        private Device device;

        private DeviceInterface deviceInterface;

        public DeviceRegister(DeviceRegisterConf deviceRegisterConf, string accessToken)
        {
            this.deviceRegisterConf = deviceRegisterConf;

            deviceInterface = new DeviceInterface(deviceRegisterConf.apiEndpoint, accessToken);
        }

        public void register()
        {
            string projectId = deviceRegisterConf.projectId;

            deviceModel = registerModel(projectId);

            device = registerInstance(projectId, deviceModel.deviceModelId);
        }

        public DeviceModel getDeviceModel()
        {
            return deviceModel;
        }

        public Device getDevice()
        {
            return device;
        }

        private T readFromFile<T>(string filePath, Type targetClass)
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

        private DeviceModel registerModel(string projectId) {

            DeviceModel optionalDeviceModel = readFromFile<DeviceModel>(deviceRegisterConf.deviceModelFilePath, typeof(DeviceModel));
            if (optionalDeviceModel != null)
            {
                Logger.Get().Debug("Got device model from file");
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
            // https://developers.google.com/assistant/sdk/reference/device-registration/model-and-instance-schemas#device_model_json
            // Light does not fit this project but there is nothing better in the API
            devModel.deviceType = "action.devices.types.AUTO";
            devModel.manifest = manifest;

            try {
                Logger.Get().Debug("Creating device model");
                DeviceModel response = deviceInterface.registerModel(projectId, devModel);

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
                Logger.Get().Error("Error during registration of the device model " + e);
            }

            return null;
        }


        private Device registerInstance(string projectId, string modelId)
        {
            Device optionalDevice = readFromFile<Device>(deviceRegisterConf.deviceInstanceFilePath, typeof(Device));
            if (optionalDevice != null)
            {
                Logger.Get().Debug("Got device from file");
                return optionalDevice;
            }


            Device dev = new Device();
            dev.id = Guid.NewGuid().ToString();
            dev.modelId = modelId;

            // Here we use the Google Assistant Service
            dev.clientType = "SDK_SERVICE";

            try {
                Logger.Get().Debug("Creating device instance");
                Device response = deviceInterface.registerDevice(projectId, dev);
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
                Logger.Get().Error("Error during registration of the device instance " + e);
            }

            return null;
        }
    }
}
