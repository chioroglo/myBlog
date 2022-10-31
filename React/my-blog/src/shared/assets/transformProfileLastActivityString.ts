export const transformToDateMonthHourseMinutesString = (date: Date) => {


    const appendZeroInFrontIfHasOneDigit = (num: number): string => {
        return num.toString().length === 1 ? "0" + num.toString() : num.toString()
    }


    return `${date.getDate()} ${date.toLocaleString('en-US', {month: 'long'})} ${date.getFullYear()} ${appendZeroInFrontIfHasOneDigit(date.getHours())}:${appendZeroInFrontIfHasOneDigit(date.getMinutes())}`;
}  