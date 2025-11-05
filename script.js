const express = require('express')
const mysql = require('mysql2/promise')
const cors = require('cors')


const app = express()
app.use(express.json())
const port = 3000

const pool = mysql.createPool({
    host: 'localhost',
    user: 'root',
    database: 'tokfejek',
    password: ''
})

app.post("/api/start/:nev",  async (req,res) => {
    const nev = req.params.nev
    try {
        const[result] = await pool.query("SELECT * FROM felhasznalok WHERE nev=?", [nev])
        if(result.length===0) {
            const [insertResult] = await pool.query("INSERT INTO felhasznalok (nev,pont) VALUES(?,0)", [nev])
            res.send({
                azon: insertResult.insertId,
                nev:nev,
                pont:0
            })
        }else{
            res.send(result[0])
        }
    } catch (error) {
        res.send(500).send("Szerverhiba"+error)
    }
})











app.listen(port, () => {
    console.log(`Example app listening on port ${port}`)
})