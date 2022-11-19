export interface CommentModel {
    id: number,
    registrationDate: string,
    postId: number,
    postTitle: string,
    authorId: number,
    authorUsername: string,
    content: string
}