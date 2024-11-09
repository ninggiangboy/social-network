package dev.ngb.spamchecking;

import jakarta.annotation.PostConstruct;
import org.pmml4s.model.Model;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.core.io.ClassPathResource;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import java.io.IOException;
import java.util.HashMap;
import java.util.Map;

@RestController
@SpringBootApplication
public class SpamCheckingApplication {

    public static void main(String[] args) {
        SpringApplication.run(SpamCheckingApplication.class, args);
    }

    private Model model;
    private static final double SPAM_THRESHOLD = 0.6;

    @PostConstruct
    public void init() throws IOException {
        ClassPathResource resource = new ClassPathResource("comment-model.pmml");
        model = Model.fromInputStream(resource.getInputStream());
    }

    @GetMapping("/spam-check")
    public boolean spamCheck(@RequestParam String content) {
        Map<String, Object> input = new HashMap<>();
        input.put("free_text", content);
        Map<?, ?> results = model.predict(input);
        double spamProbability = (double) results.get("probability(1)");
        return spamProbability > SPAM_THRESHOLD;
    }
}
