// TODO MAKE FUNCTION TRANSFORM UTC TO USERS LOCAL


export const transformUtcStringToDateMonthHoursMinutesString = (dateStringUtc: string) => {

    const localDate = new Date(Date.parse(dateStringUtc));

    // console.log(dateUtc.toLocaleString());
    // console.log(dateUtc.getTimezoneOffset());
    // console.log(new Date().getTimezoneOffset());
    //console.log(Intl.DateTimeFormat().resolvedOptions().timeZone);
    //console.log(new Date(Date.now()));
    //console.log(dateUtc.toUTCString());
    //console.log(dateUtc);

    //const options: Intl.DateTimeFormatOptions = {
    //    timeZone: Intl.DateTimeFormat().resolvedOptions().timeZone,
    //    hour: "2-digit",
    //    minute:"2-digit",
    //    day: "2-digit",
    //    year: "numeric",
    //    month: "short",
    //}


    return `${localDate.getDate()} ${localDate.getMonth()}`;
    //return dateUtc.toLocaleTimeString('en-UK',options);
}  