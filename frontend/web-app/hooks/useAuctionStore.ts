import { Auction, PagedResult } from "@/types"
import { create } from "domain"

type State = {
    auctions: Auction[]
    totalCount: number
    pageCount: number
}

type Actions = {
    setData: (data: PagedResult<Auction>) => void
    setCurrentPrice: (auctionId: string, amount: number) => void
}

const initialState: State = {
    auctions:[],
    pageCount: 0,
    totalCount: 0,
}

// export const useAuctionStore = create<State & Actions>((set) => ({
//     ...initialState,

//     setData: () => {},

//     setCurrentPrice: () => {},
// }))