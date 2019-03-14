export interface Word {
  wordId: number
  word: string,
  description?: string,
  daily?: number,
  weekly?: number,
  yearly?: number,
  main?: boolean,
  type?: number
}