export const getFirstCharOfStringUpperCase = (str: string | null | undefined) => {
    if (str) {
        return str[0].toUpperCase();
    }
    else {
        return "";
    }
}