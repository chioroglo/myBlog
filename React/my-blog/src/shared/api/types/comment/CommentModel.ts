export interface CommentModel {
    id: number,
    registrationDate: Date,
    postId: number,
    postTitle: string,
    authorId: number,
    authorUsername: string,
    content: string
}