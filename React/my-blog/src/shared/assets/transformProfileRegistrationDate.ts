

// TODO MAKE FUNCTION TRANSFORM UTC TO USERS LOCAL
export const transformToDayMonthYear = (date: Date) => {
    return `${date.getDate()} ${date.toLocaleString('en-US', {month: 'long'})} ${date.getFullYear()}`;
}  