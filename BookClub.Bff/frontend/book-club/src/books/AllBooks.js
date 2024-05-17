import React, {useEffect, useState} from "react";
import { Books } from "./books.service";
import {BookTile} from "./BookTile";

export const AllBooks = () => {
    var [books, setBooks] = useState([])
    useEffect(() => {
        console.log('in use effect')
        Books.getBooks().then(setBooks)
    }, [setBooks])
    
    return (
    <>
        <h1>All Books</h1>
        {books.forEach(book => <BookTile book ={book}/>)}
    </>
    )
}