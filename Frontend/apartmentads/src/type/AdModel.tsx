interface AdModel {
    id : number;
    title : string;
    description : string;
    date : string;
    price : number;
    email? : string;
    apartmentId? : number;
}

export default AdModel;