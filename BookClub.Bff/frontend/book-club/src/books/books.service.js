import { Http } from "../services/http";

export const Books = {
    getBooks: async params => {
        return await Http.get('Books/GetBooks', {params})
    }
}
