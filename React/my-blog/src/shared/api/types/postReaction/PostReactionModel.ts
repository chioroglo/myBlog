import {ReactionType} from "../ReactionType";

export interface PostReactionModel {
    "reactionType": ReactionType,
    "userId": number,
    "postId": number
}