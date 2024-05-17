import React from "react";

export const BookTile = ({title, author, description, picture}) => {
    return (<>
    <h2>{title}</h2>
    <h3>{author}</h3>
    <body>{description}</body>
    </>)

}