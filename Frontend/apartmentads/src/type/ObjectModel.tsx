interface ObjectModel {
    id: number;
    name: string;
    description: string;
    image: string;
    grade: number;
    apartmentId?: number;
    roomId?: number;
}

export default ObjectModel;