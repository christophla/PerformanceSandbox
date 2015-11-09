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
var http = require('http');
var Sequelize = require('sequelize');

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


// ****************************************************************************
// Start server
//

var app = express();

app
    .use('/', router)
    .use(bodyParser.urlencoded({ extended: true }))
    .use(bodyParser.json())
    .listen(7902, function() {
        console.log('Running at PORT 7902');
    });

console.log('Application running!');