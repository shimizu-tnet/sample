import axios from 'axios';

const baseUrl = 'https://sensei-admin.legacy.ne.jp/api';
const proposalList = `${baseUrl}/proposal/list`;
const proposalRefer = `${baseUrl}/proposal/refer`;
const proposalSave = `${baseUrl}/proposal/save`;

// 案件一覧取得
export const fetchList = async (ankenId, pageNo, platformToken) => {
  const response = await axios({
    url: `${proposalList}?ankenId=${ankenId}&page=${pageNo}`,
    method: 'get',
    headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json; charset=utf-8',
      'Authorization': platformToken,
    },
    responseType: 'json',
  });

  return response.data;
}

// JSON取得
export const fetchRefer = async (ankenId, proposalId, platformToken) => {
  const response = await axios({
    url: proposalRefer,
    method: 'post',
    headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json; charset=utf-8',
      'Authorization': platformToken,
    },
    data: { ankenId, proposalId },
    responseType: 'json',
  });

  return response.data;
}

// PDF登録
export const savePdf = async (list, initialReport, estimateReport, json) => {
  const { ankenId, platformToken } = list;
  const sendingParameters = {
    ankenId,
    proposalId: null,
    jsonData: JSON.parse(json),
  }
  if (initialReport) {
    sendingParameters.proposalDataUri = await getBase64DataUri(initialReport);
  }
  if (estimateReport) {
    sendingParameters.estimateDataUri = await getBase64DataUri(estimateReport);
  }

  await axios({
    url: proposalSave,
    method: 'post',
    headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json; charset=utf-8',
      'Authorization': platformToken,
    },
    data: sendingParameters,
    responseType: 'json',
  });
}

const getBase64DataUri = report => {
  return new Promise(resolve => {
    const reader = new FileReader();
    reader.onload = () => resolve(reader.result);
    reader.readAsDataURL(new Blob([report], { type: 'application/pdf' }));
  });
}
