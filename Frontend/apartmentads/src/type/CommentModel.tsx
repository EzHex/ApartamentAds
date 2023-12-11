interface CommentModel {
    id: number;
    content: string;
    apartmentId?: number;
    roomId?: number;
    objectId?: number;
}

export default CommentModel;