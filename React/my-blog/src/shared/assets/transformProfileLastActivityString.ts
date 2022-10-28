export const transformProfileLastActivityDate = (date:Date) => {
    return `${date.getDate()} ${date.toLocaleString('en-US',{month:'long'})} ${date.getFullYear()} ${date.getHours()}:${date.getMinutes()}`;
}  