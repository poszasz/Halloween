const express = require('express')
const mysql = require('mysql2/promise')
const cors = require('cors')


const app = express()
app.use(express.json())
const port = 3000

app.use(cors({
    origin: '*',
    credentials: true
}))

const pool = mysql.createPool({
    host: 'localhost',
    user: 'root',
    database: 'tokfejek',
    password: ''
})


function esemenyElkeszitese(){
    let lista =[];
    const ido =60;
    const itervallum = 5;

    for (let i = 0; i < 60 ; i+=itervallum) {
        const tokAzon = Math.floor(Math.random()*5)
        const erteke = Math.floor(Math.random()*3)
        const nyitasIdo = i;
        const zarasIdo = i+0.7
        lista.push({
            tokAzon:tokAzon,
            erteke:erteke,
            nyitasIdo:nyitasIdo,
            zarasIdo:zarasIdo
        })
        
    }
    return lista;
}

app.post("/api/start/:nev",  async (req,res) => {
    const nev = req.params.nev;
    try {
        const[result] = await pool.query("SELECT * FROM felhasznalok WHERE nev=?", [nev])
        if(result.length===0) {
            const [insertResult] = await pool.query("INSERT INTO felhasznalok (nev,pont) VALUES(?,0)", [nev])
            res.send({
                azon: insertResult.insertId,
                nev:nev,
                pont:0,
                esemenyek: esemenyElkeszitese()
            })
        }else{
            res.send(result[0]).send({
                azon:result[0].azon,
                nev:result[0].nev,
                pont:result[0].pont,
                esemenyek: esemenyElkeszitese()
            })
              
        }
    } catch (error) {
        res.send(500).send("Szerverhiba"+error)
    }
})

app.post("/api/score/:azon", async (req,res) => {
    const azon = req.params.azon;
    const pont = req.body.pont;
    if(!pont){
        res.status(400).send("Hibás/hiányos bemeneti adat!")
    }
    try {
        const[result]  = await pool.query("SELECT pont FROM felhasznalok WHERE azon=? ", [azon])
        const jelenlegiPontszam = parseInt(result[0].pont)
        if(jelenlegiPontszam<parseInt(pont)){
            const[result]  = await pool.query("UPDATE felhasznalok SET pont=? WHERE azon=?", [pont,azon])
            res.status(200).send("Új rekord!")
        }
        else{
            res.status(200).send("Ez nem új rekord!")
        }
    } catch (error) {
        res.status(500).send("Szerverhiba" + error)
    }


})

app.get("/api/leaderboard", async (req,res) => {
    try {
        const[result] = await pool.query("SELECT nev, pont FROM felhasznalok ORDER BY pont DESC")
        res.status(200).send(result)
    } catch (error) {
        res.status(500).send("Szerverhiba!" + error)
    }
})

app.listen(port, () => {
    console.log(`Example app listening on port ${port}`)
})
