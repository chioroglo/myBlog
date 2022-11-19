// TODO MAKE FUNCTION TRANSFORM UTC TO USERS LOCAL
export const transformUtcToLocalDate = (dateStringUtc: string) => {

    const localDate = new Date(Date.parse(dateStringUtc));
    return localDate.toLocaleString('en-UK',{ day:"2-digit", month:"long",year:"numeric"});
    //return `${dateStringUtc.getDate()} ${dateStringUtc.toLocaleString('en-US', {month: 'long'})} ${dateStringUtc.getFullYear()}`;
}  