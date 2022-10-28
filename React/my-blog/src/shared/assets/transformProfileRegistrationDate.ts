export const transformProfileRegistrationDate = (date:Date) => {
    return `${date.getDate()} ${date.toLocaleString('en-US',{month:'long'})} ${date.getFullYear()}`;
}  