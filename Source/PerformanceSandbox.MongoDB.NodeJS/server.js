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
var mongoose = require('mongoose');
var Schema = mongoose.Schema;
var app = express();

app.use(bodyParser.urlencoded({ extended: true }));
app.use(bodyParser.json());

var port = 7905; // set our port


// ****************************************************************************
// Database connection
//

var mongoose = require('mongoose');
mongoose.connect('mongodb://localhost/PerformanceSandbox'); // connect to our database


// ****************************************************************************
// Product model
//

var Product = require('./models/product');


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
        var query = Product.find({ 'Archived': false }).limit(30);

        query.exec(function(err, products) {
            if (err)
                res.send(err);

            res.json(products);
        });
    });


// ****************************************************************************
// GET /products/:products_id
//

router.route('/products/:product_id')
    .get(function(req, res) {
        Product.findById(req.params.bear_id, function(err, product) {
            if (err)
                res.send(err);
            res.json(product);
        });
    });

app.use('/', router);


// ****************************************************************************
// Start server
//

app.listen(port);
console.log(`Magic happens on port ${port}`);