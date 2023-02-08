//
// axiosSettings
//

const baseURL = process.env.VUE_APP_API_SERVICE_HOST || "";

const axiosSettings = {
  requestConfig: {
    baseURL,
    headers: {
      "Content-Type": "application/json",
    },
  },
  mock: {
    enable: true,
    adapterOptions: {
      delayResponse: 600,
    },
  },
};

const axiosExportSettings = {
  requestConfig: {
    baseURL,
    headers: {
      'Content-Type': 'application/json',
      Pragma: 'no-cache',
      Accept: 'application/json',
    },
    responseType: 'arraybuffer'
  },
  mock: {
    enable: true,
    adapterOptions: {
      delayResponse: 600,
    },
  },
};

//
// export
//

export { axiosSettings, axiosExportSettings };
