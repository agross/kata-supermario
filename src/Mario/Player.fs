module Player


type Size =
  | Small
  | Dead

let player = Size.Small

let hit _ = Size.Dead
