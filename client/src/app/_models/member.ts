import { Photo } from './photo';
export interface Member {
  id:          number;
  userName:    string;
  photoUrl:    string;
  createdTime: Date;
  lastActive:  Date;
  age:         number;
  gender:      string;
  adress:      string;
  description: string;
  photos:      Photo[];
}

