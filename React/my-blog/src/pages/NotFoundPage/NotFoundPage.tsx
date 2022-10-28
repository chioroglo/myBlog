import { Link } from "react-router-dom"

const NotFoundPage = () => {
    return (<div style={{display:"block",alignItems:"center",margin:"25vh 25vw"}}>
        <h1>The existing URL was not found on this server.</h1>
        <Link style={{fontStyle:"italic",display:"block",textAlign:"center"}} to="/">Go home</Link>
    </div>)
}

export {NotFoundPage}