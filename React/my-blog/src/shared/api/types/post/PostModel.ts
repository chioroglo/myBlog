export interface PostModel {
    "id": number,
    "registrationDate": Date,
    "title": string,
    "content": string,
    "authorId": number,
    "amountOfComments": number,
    "authorUsername": string,
    "topic"?: string
  }