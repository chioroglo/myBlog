export const getFirstCharOfString = (str: string | null | undefined) => {
    if (str) {
        return str[0];
    }
    else {
        return "";
    }
}