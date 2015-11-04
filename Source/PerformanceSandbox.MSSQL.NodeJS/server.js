//=============================================================================
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//=============================================================================


// ****************************************************************************
// Configure
//

var express = require('express');
var bodyParser = require('body-parser');
var Sequelize = require('sequelize');
var app = express();

app.use(bodyParser.urlencoded({ extended: true }));
app.use(bodyParser.json());

var port = 7902; // set our port


// ****************************************************************************
// Database connection
//

var sequelize = new Sequelize('PerformanceSandbox', 'sandbox', 'p@ssw0rd', {
    host: '127.0.0.1',
    dialect: 'mssql',
    pool: {
        max: 5,
        min: 0,
        idle: 10000
    },
    dialectOptions: {
        encrypt: true
    }
});


// ****************************************************************************
// Product model
//

var Product = sequelize.define('product', {
    id: {
        type: Sequelize.BIGINT,
        primaryKey: true
    },
    archived: { type: Sequelize.BOOLEAN },
    createdAt: { type: Sequelize.DATE },
    createdBy: { type: Sequelize.STRING },
    description: { type: Sequelize.STRING },
    modifiedAt: { type: Sequelize.DATE },
    modifiedBy: { type: Sequelize.STRING },
    name: { type: Sequelize.STRING },
    quantityAvailable: { type: Sequelize.INTEGER },
}, {
    freezeTableName: true,
    updatedAt: 'modifiedAt'
});


// ****************************************************************************
// Routes
//

var router = express.Router();

router.use((req, res, next) => {
    console.log('Request in-progess...');
    next();
});

router.get('/', (req, res) => {
    res.json({ message: 'Performance Sandbox API' });
});

// ****************************************************************************
// GET /products
//

router.route('/products')
    .get((req, res) => {
        Product.findAll().then((product) => {
            res.json(product);
        });
});


// ****************************************************************************
// GET /products/:products_id
//

router.route('/products/:product_id')
    .get(function(req, res) {
        Product.findAll({
            where: {
                id: req.params.product_id
            }
        }).then((product) => {
            res.json(product);
        });
    });

app.use('/', router);


// ****************************************************************************
// Start server
//

app.listen(port);
console.log(`Magic happens on port ${port}`);