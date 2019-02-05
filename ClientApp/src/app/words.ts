export interface Word {
  wordId: number
  word: string,
  daily?: number,
  weekly?: number,
  yearly?: number,
  main?: boolean,
  type?: number
}