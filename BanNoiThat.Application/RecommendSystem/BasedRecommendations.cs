using BanNoiThat.Domain.Interface;

namespace BanNoiThat.Application.Service.RecommendSystem
{
    public class BasedRecommendations<T> where T : IEntityRecommend
    {
        private List<string> _vocabularyKeyword;

        public BasedRecommendations(List<string> vocabularyKeyword)
        {
            _vocabularyKeyword = vocabularyKeyword;
        }

        public List<T> GetContentBasedRecommendations(string[] interactedProductIds, List<T> productsData, List<Dictionary<string, double>> tfidfVectors)
        {
            if (interactedProductIds == null || interactedProductIds.Length == 0)
                return new List<T>();

            // Lấy vector TF-IDF của các sản phẩm đã tương tác
            var interactedVectors = new List<Dictionary<string, double>>();
            foreach (var id in interactedProductIds)
            {
                var index = GetProductIndexById(id, productsData);
                if (index != -1)
                {
                    interactedVectors.Add(tfidfVectors[index]);
                }
            }

            // Tính điểm tương đồng cho tất cả sản phẩm
            var scores = new List<(T Product, double Score)>();
            for (int i = 0; i < tfidfVectors.Count; i++)
            {
                var productId = GetProductIdByIndex(i, productsData);
                //if (interactedProductIds.Contains(productId)) continue; // Bỏ qua sản phẩm đã tương tác

                double totalSimilarity = 0;
                foreach (var interactedVector in interactedVectors)
                {
                    totalSimilarity += CosineSimilarity(tfidfVectors[i], interactedVector);
                }

                double avgSimilarity = totalSimilarity / interactedVectors.Count;
                scores.Add((productsData[i], avgSimilarity));
            }

            // Sắp xếp sản phẩm theo điểm số giảm dần
            scores = scores.OrderByDescending(score => score.Score).ToList();

            var modelsReturn =  scores.Select(score => score.Product).ToList();
            return modelsReturn;
        }

        // Hàm hỗ trợ: Lấy index sản phẩm theo ID
        private int GetProductIndexById(string productId, List<T> productsData)
        {
            return productsData.FindIndex(product => product.Id == productId);
        }

        // Hàm hỗ trợ: Lấy ID sản phẩm theo index
        private string GetProductIdByIndex(int index, List<T> productsData)
        {
            return productsData[index].Id;
        }

        #region Hỗ trợ tính
        public List<Dictionary<string, double>> ComputeTFIDF(List<Dictionary<string, double>> tfArray, Dictionary<string, double> idf)
        {
            // Danh sách kết quả TF-IDF
            var tfidfArray = new List<Dictionary<string, double>>();

            foreach (var tf in tfArray)
            {
                var tfidf = new Dictionary<string, double>();

                foreach (var term in tf.Keys)
                {
                    // Tính TF-IDF: TF[term] * IDF[term]
                    tfidf[term] = tf[term] * idf[term];
                }

                tfidfArray.Add(tfidf);
            }

            return tfidfArray;
        }

        public List<Dictionary<string, double>> ComputeTF(List<T> products)
        {
            // Danh sách lưu trữ kết quả TF cho từng sản phẩm
            var tfArray = new List<Dictionary<string, double>>();

            foreach (var product in products)
            {
                // Lấy danh sách từ khóa của sản phẩm
                string[] keywords = product.Keyword.Split(' ');

                // Từ điển để lưu TF cho từng từ khóa trong vocabulary
                var tf = new Dictionary<string, double>();

                foreach (var term in _vocabularyKeyword)
                {
                    // Đếm số lần xuất hiện của term trong danh sách từ khóa
                    var termCount = keywords.Count(k => k == term);

                    // Tính TF: số lần xuất hiện chia cho tổng số từ khóa
                    tf[term] = keywords.Length > 0 ? (double)termCount / keywords.Length : 0.0;
                }

                // Thêm từ điển TF vào danh sách kết quả
                tfArray.Add(tf);
            }

            return tfArray;
        }

        public Dictionary<string, double> ComputeIDF(List<T> products)
        {
            // Từ điển lưu giá trị IDF cho từng term
            var idf = new Dictionary<string, double>();

            // Tổng số sản phẩm
            int totalDocs = products.Count;

            foreach (var term in _vocabularyKeyword)
            {
                // Đếm số lượng sản phẩm chứa từ khóa term
                int docsWithTerm = 0;
                foreach (var product in products)
                {
                    string[] keywords = product.Keyword.Split(' ');
                    if (keywords.Contains(term))
                    {
                        docsWithTerm++;
                    }
                }

                // Tính IDF: log10(totalDocs / (1 + docsWithTerm))
                idf[term] = Math.Log10((double)totalDocs / (1 + docsWithTerm));
            }

            return idf;
        }

        // Hàm tính chỉ số cosine similarity giữa hai vector TF-IDF
        private double CosineSimilarity(Dictionary<string, double> vectorA, Dictionary<string, double> vectorB)
        {
            double dotProduct = 0, magnitudeA = 0, magnitudeB = 0;

            foreach (var term in vectorA.Keys)
            {
                if (vectorB.ContainsKey(term))
                {
                    dotProduct += vectorA[term] * vectorB[term];
                }
                magnitudeA += Math.Pow(vectorA[term], 2);
            }

            foreach (var term in vectorB.Values)
            {
                magnitudeB += Math.Pow(term, 2);
            }

            magnitudeA = Math.Sqrt(magnitudeA);
            magnitudeB = Math.Sqrt(magnitudeB);

            return magnitudeA * magnitudeB == 0 ? 0 : dotProduct / (magnitudeA * magnitudeB);
        }
        #endregion
    }
}
