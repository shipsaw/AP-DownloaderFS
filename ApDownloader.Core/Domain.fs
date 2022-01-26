namespace Domain


type FileStatus =
    | IsMissing
    | CanUpdate
    | Ok

type CurrentPage =
    | LoginPage
    | DownloadPage
    | InstallPage
    | OptionsPage

type DownloadConfig =
    { GetExtraStock: bool
      GetBrandingPatch: bool
      GetLiveryPack: bool
      DownloadFilepath: string
      InstallFilepath: string }

type Product =
    { ProductId: int
      Name: string
      Filename: string
      ImageName: string
      EsFilename: string option
      BpFilename: string option
      LpFilename: string option }

type ProductListItem =
    { ProductId: int
      ImageUrl: string
      Name: string
      CanUpdate: bool
      IsNotOnDisk: bool }
