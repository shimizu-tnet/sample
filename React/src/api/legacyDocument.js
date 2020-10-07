import axios from 'axios';
import { hasResponseError, hasResponseStatus } from '../features/Errors/ErrorSlice';
if (document.URL.indexOf("localhost:44339") !== -1) {
  axios.defaults.baseURL = "https://localhost:44339/";
}

export async function calculateInheritanceTax(inheritanceTaxParameters) {
  const {
    accessToken,
    inheritanceTotalAmount,
    propertyTotalAmount,
    relatives,
    heirPattern,
    estimatedHeirsCount
  } = inheritanceTaxParameters;
  const params = new URLSearchParams();
  params.append('value', JSON.stringify({
    heirPattern: heirPattern,
    relatives: relatives,
    estimatedInheritanceTax: {
      inheritanceTotalAmount: inheritanceTotalAmount,
      propertyTotalAmount: propertyTotalAmount,
    },
    estimatedHeirsCount: estimatedHeirsCount,
  }));

  const options = {
    method: 'post',
    baseURL: getBaseUrl(),
    url: 'api/InheritanceTax',
    responseType: 'json',
    data: params,
    headers: {
      'Authorization': 'Bearer ' + accessToken
    },
  }

  return await getJsonAsync(options);
}

export async function calculateInheritanceSchedule(accessToken, schedules) {
  const params = new URLSearchParams();
  params.append('value', JSON.stringify(schedules));

  const options = {
    method: 'post',
    baseURL: getBaseUrl(),
    url: 'api/InheritanceSchedule',
    responseType: 'json',
    data: params,
    headers: {
      'Authorization': 'Bearer ' + accessToken
    },
  }

  return await getJsonAsync(options);
}

export async function calculateProposalSchedule(accessToken, schedules) {
  const params = new URLSearchParams();
  params.append('value', JSON.stringify(schedules));

  const options = {
    method: 'post',
    baseURL: getBaseUrl(),
    url: 'api/ProposalSchedule',
    responseType: 'json',
    data: params,
    headers: {
      'Authorization': 'Bearer ' + accessToken
    },
  }

  return await getJsonAsync(options);
}

export async function authenticate(user_name, password) {
  const params = new URLSearchParams();
  params.append('grant_type', 'password');
  params.append('username', user_name);
  params.append('password', password);

  const options = {
    method: 'post',
    baseURL: getBaseUrl(),
    url: 'Token',
    headers: {
      'Content-Type': 'application/x-www-form-urlencoded',
    },
    responseType: 'json',
    data: params
  }

  try {
    const json = await getJsonAsync(options);
    return json.access_token;
  } catch (err) {
    if (hasResponseError(err) && hasResponseStatus(err.response)) {
      const status = err.response.status;
      if (status === 400 && err.response.data.error === 'invalid_grant') {
        throw new Error('ログインに失敗しました。\nID またはパスワードを確認してください。');
      } else {
        throw new Error('処理でエラーが発生したため、再度実行してください。');
      }
    }

    throw new Error('処理でエラーが発生したため、再度実行してください。');
  }
}

export const getPdf = async (type, paramValues, accessToken) => {
  const params = new URLSearchParams();
  params.append('value', paramValues);

  const options = {
    method: 'post',
    baseURL: getBaseUrl(),
    url: `api/${type}`,
    headers: {
      'Authorization': 'Bearer ' + accessToken,
    },
    responseType: 'arraybuffer',
    data: params
  }

  return await getPdfAsync(options);
}

export async function getJsonAsync(options) {
  const res = await executeApiAsync(options);
  const json = await res.data;
  return json;
}

export async function getPdfAsync(options) {
  const res = await executeApiAsync(options);
  const blob = new Blob([res.data], { type: 'application/pdf' });
  return blob;
}

export async function executeApiAsync(options) {
  const res = await axios(options)
  return res;
}

const getBaseUrl = () => {
  const origin = window.location.origin;
  if (origin === 'http://localhost:3000') {
    return 'http://localhost';
  }

  return '/';
}
