export const transformUtcStringToDateMonthHoursMinutesString = (dateStringUtc: string) => {

    const localDate = new Date(Date.parse(dateStringUtc));


    return localDate.toLocaleString('en-UK', {day: "2-digit", month: "long", hour: "2-digit", minute: "2-digit"});
}