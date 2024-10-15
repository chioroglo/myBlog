export interface PostModel {
    id: number,
    registrationDate: string
    title: string
    content: string
    authorId: number
    amountOfComments: number
    authorUsername: string
    topic?: string
    language?: string
}