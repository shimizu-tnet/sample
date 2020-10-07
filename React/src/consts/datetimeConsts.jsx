export const todayNYear = (new Date()).getFullYear();
export const todayNMonth = (new Date()).getMonth() + 1;
export const todayNDay = (new Date()).getDate();
export const todaySYear = `${todayNYear}`;
export const todaySMonth = `0${todayNMonth}`.slice(-2);
export const todaySDay = `0${todayNDay}`.slice(-2);
