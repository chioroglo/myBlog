export const stringToByteArray = (str: string): ArrayBuffer => {
    const challengeBinary = atob(str);
    const challengeBytes = new Uint8Array(challengeBinary.length)
  
    for (let i = 0; i < challengeBinary.length; i++) {
      challengeBytes[i] = challengeBinary.charCodeAt(i);
    }
    return challengeBytes;
}

export const arrayBufferToBase64 = (buffer: ArrayBuffer) => btoa(String.fromCharCode(...new Uint8Array(buffer)));
  
export const arrayBufferToUtf8 = (buffer: ArrayBuffer) => String.fromCharCode(...new Uint8Array(buffer));
  