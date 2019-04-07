import { Images } from "./images";

export interface Stories {
  wordId: number
  word: string,
  daily?: number,
  weekly?: number,
  yearly?: number,
  main?: boolean,
  type?: number,
  images: Images[],
  dateIncremented: Date
}